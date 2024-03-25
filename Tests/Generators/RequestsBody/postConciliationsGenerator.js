import * as dotenv from "dotenv";
import knex from "knex";
import {BODY_TEST_FILE_PATH, saveDataToJson} from "../../utils.js";

dotenv.config();

const db = knex({
    client: "pg",
    connection: process.env.DATABASE_URL,
});

export default async function createBody() {
    try {
        const [{token}] = await db
            .select("token")
            .from("payment_provider_tokens")
            .limit(1);

        const requests = {
            token,
            payload: {
                date: "2024-03-20",
                file: "paymentsDb.ndjson",
                postback: "http://pspapi:8080/conciliation",
            }
        };

        const filepath = `${BODY_TEST_FILE_PATH}/postConciliations.json`;

        await saveDataToJson(requests, filepath);
    } catch (error) {
        console.error("An error occurred during body generate:", error);
    } finally {
        await db.destroy();
    }
}
