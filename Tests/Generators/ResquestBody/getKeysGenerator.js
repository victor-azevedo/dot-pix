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
        const keys = await db.select("type", "value").from("pix_keys").limit(BODY_TEST_LENGTH);

        const [{ token }] = await db.select("token").from("payment_provider_tokens").limit(1);

        const requests = keys.map((key) => ({
            token,
            payload: {
                type: key.type,
                value: key.value,
            },
        }));

        const filepath = `${BODY_TEST_FILE_PATH}/getKeys.json`;

        await saveDataToJson(requests, filepath);
    } catch (error) {
        console.error("An error occurred during body generate:", error);
    } finally {
        await db.destroy();
    }
}

await init();
