import { faker } from "@faker-js/faker";
import * as dotenv from "dotenv";
import { UniqueEnforcer } from "enforce-unique";
import knex from "knex";
import { BODY_TEST_FILE_PATH, BODY_TEST_LENGTH, saveDataToJson } from "../../utils.js";

dotenv.config();

const db = knex({
    client: "pg",
    connection: process.env.DATABASE_URL,
});

const uniqueEnforcerUudi = new UniqueEnforcer();

async function init() {
    try {
        const users = await db.select("cpf").from("users").limit(BODY_TEST_LENGTH);

        const accounts = await db.select("account", "agency").from("payment_provider_accounts").limit(BODY_TEST_LENGTH);

        const paymentProviderTokens = await db.select("token").from("payment_provider_tokens").limit(BODY_TEST_LENGTH);

        const requests = users.map((user, index) => ({
            token: paymentProviderTokens[index].token,
            payload: {
                key: createKey(),
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
        uniqueEnforcerUudi.reset();
        await db.destroy();
    }
}

function createKey() {
    const value = uniqueEnforcerUudi.enforce(() => {
        return faker.string.uuid();
    });
    const type = "Random";
    return { value, type };
}

await init();
