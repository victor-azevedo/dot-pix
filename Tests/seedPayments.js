import * as dotenv from "dotenv";
import knex from "knex";
import {SEED_FILE_PATH, addDateToData, readDataFromJson} from "./utils.js";
import createPaymentsJson from "./Generators/paymentsGenerator.js";

dotenv.config();

const PAYMENTS_TO_GENERATE = 20000000

const db = knex({
    client: "pg",
    connection: process.env.DATABASE_URL,
});

async function init() {
    try {
        while (paymentsInDbCount < PAYMENTS_TO_GENERATE) {
            await createPaymentsJson();

            paymentsInDbCount = await tableCount("payments");

            await seedPayments();
        }
    } catch
        (error) {
        console.error("An error occurred during database initialization:", error);
    } finally {
        await db.destroy();
    }
}

async function seedPayments() {
    const tableName = "payments";

    const ACCOUNTS_NUMBER = 2

    function choiceAccount(number) {
        return number % ACCOUNTS_NUMBER;
    }

    const payments = await readDataFromJson(`${SEED_FILE_PATH}/payments.json`);

    const paymentProvidersAccountsDb = await db.select("id").from("payment_provider_accounts").limit(ACCOUNTS_NUMBER);

    const pixKeysDb = await db.select("id").from("pix_keys");

    if (payments.length > pixKeysDb.length)
        throw new Error("Key array size mismatch: Verify the number of accounts.");

    const paymentsToDb = payments.map((payment, index) => ({
        ...payment,
        account_origin_id: paymentProvidersAccountsDb[choiceAccount(index)].id,
        pix_key_destiny_id: pixKeysDb[pixKeysDb.length - index - 1].id,
    }));

    await seedDb(tableName, paymentsToDb);
}

async function seedDb(tableName, data) {
    console.log(`Seeding database, '${tableName}' table...`);

    await db.batchInsert(tableName, addDateToData(data));

    console.log(`Seeded '${tableName}' table with success!`);
    console.log("\n*****************************************\n");
}

async function tableCount(tableName) {
    console.log(`Counting ${tableName}`);

    const countResult = await db(tableName).count('* as total');
    const totalCount = countResult[0].total;

    console.log(`Total count of elements in '${tableName}': ${totalCount}`);
    console.log("\n*****************************************\n");
    return totalCount
}


let paymentsInDbCount = await tableCount("payments")
await init();

