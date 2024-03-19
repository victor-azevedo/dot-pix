import * as dotenv from "dotenv";
import knex from "knex";
import { BODY_TEST_FILE_PATH, BODY_TEST_LENGTH, saveDataToJson } from "../../utils.js";

dotenv.config();

const db = knex({
    client: "pg",
    connection: process.env.DATABASE_URL,
});

async function init() {
    try {
        const users = await db.select("cpf").from("users").limit(BODY_TEST_LENGTH);

        const accounts = await db.select("account", "agency").from("payment_provider_accounts").limit(BODY_TEST_LENGTH);

        const paymentProviderTokens = await db.select("token").from("payment_provider_tokens").limit(BODY_TEST_LENGTH);

        const requests = users.map((user, index) => ({
            token: paymentProviderTokens[index].token,
            payload: {
                key: {
                    type: "Random",
                    key: null, // generate in k6
                },
                user: {
                    cpf: user.cpf,
                },
                account: {
                    number: accounts[index].account,
                    agency: accounts[index].agency,
                },
            },
        }));

        const filepath = `${BODY_TEST_FILE_PATH}/postKeys.json`;

        await saveDataToJson(requests, filepath);
    } catch (error) {
        console.error("An error occurred during body generate:", error);
    } finally {
        await db.destroy();
    }
}

await init();
