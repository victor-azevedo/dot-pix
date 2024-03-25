import {faker} from "@faker-js/faker";
import * as dotenv from "dotenv";
import knex from "knex";
import {BODY_TEST_FILE_PATH, BODY_TEST_LENGTH, saveDataToJson} from "../../utils.js";

dotenv.config();

const db = knex({
    client: "pg",
    connection: process.env.DATABASE_URL,
});

export default async function createBody() {
    try {
        const [user] = await db.select("id", "cpf").from("users").limit(1);

        const [account] = await db
            .select("account", "agency", "payment_provider_id")
            .from("payment_provider_accounts")
            .where("user_id", user.id)
            .limit(1);

        const [{token}] = await db
            .select("token")
            .from("payment_provider_tokens")
            .where("payment_provider_id", account.payment_provider_id)
            .limit(1);

        const keys = await db.select("type", "value").from("pix_keys").limit(BODY_TEST_LENGTH);

        const requests = keys.map((key) => ({
            token,
            payload: {
                origin: {
                    user: {
                        cpf: user.cpf,
                    },
                    account: {
                        number: account.account,
                        agency: account.agency,
                    },
                },
                destiny: {
                    key: {
                        value: key.value,
                        type: key.type,
                    },
                },
                amount: 0, // generate in k6
                description: faker.word.adjective(),
            },
        }));

        const filepath = `${BODY_TEST_FILE_PATH}/postPayments.json`;

        await saveDataToJson(requests, filepath);
    } catch (error) {
        console.error("An error occurred during body generate:", error);
    } finally {
        await db.destroy();
    }
}
