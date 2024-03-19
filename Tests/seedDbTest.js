import * as dotenv from "dotenv";
import knex from "knex";
import { SEED_FILE_PATH, addDateToData, readDataFromJson } from "./utils.js";

dotenv.config();

const db = knex({
    client: "pg",
    connection: process.env.DATABASE_URL,
});

const args = process.argv.slice(2);
const isToCleanDb = args[0] === "clean";

async function init() {
    try {
        if (isToCleanDb) {
            await clearDb();
        }
        await seedUsers();
        await seedPaymentsProviders();
        await seedPaymentsProviderTokens();
        await seedPaymentsProviderAccounts();
        await seedPixKeys();
        await seedPayments();
    } catch (error) {
        if (isToCleanDb) {
            await clearDb();
        }
        console.error("An error occurred during database initialization:", error);
    } finally {
        await db.destroy();
    }
}

async function seedUsers() {
    const tableName = "users";
    const users = await readDataFromJson(`${SEED_FILE_PATH}/users.json`);

    await seedDb(tableName, users);
}

async function seedPaymentsProviders() {
    const tableName = "payment_providers";

    const paymentProviders = await readDataFromJson(`${SEED_FILE_PATH}/paymentProviders.json`);

    await seedDb(tableName, paymentProviders);
}

async function seedPaymentsProviderTokens() {
    const tableName = "payment_provider_tokens";

    const paymentProviderTokens = await readDataFromJson(`${SEED_FILE_PATH}/paymentProviderTokens.json`);

    const paymentProvidersDb = await db.select("id").from("payment_providers");

    if (paymentProviderTokens.length !== paymentProvidersDb.length)
        throw new Error("Token array size mismatch: Verify the number of tokens.");

    const tokens = paymentProvidersDb.map((paymentProvider, index) => ({
        payment_provider_id: paymentProvider.id,
        token: paymentProviderTokens[index],
    }));

    await seedDb(tableName, tokens);
}

async function seedPaymentsProviderAccounts() {
    const tableName = "payment_provider_accounts";

    const paymentProviderAccounts = await readDataFromJson(`${SEED_FILE_PATH}/accounts.json`);

    const paymentProvidersDb = await db.select("id").from("payment_providers");
    const usersDb = await db.select("id").from("users");

    if (paymentProviderAccounts.length > paymentProvidersDb.length)
        throw new Error("Accounts array size mismatch: Verify the number of accounts.");

    const accounts = paymentProviderAccounts.map((account, index) => ({
        ...account,
        payment_provider_id: paymentProvidersDb[index].id,
        user_id: usersDb[index].id,
    }));

    await seedDb(tableName, accounts);
}

async function seedPixKeys() {
    const tableName = "pix_keys";

    const pixKeys = await readDataFromJson(`${SEED_FILE_PATH}/keys.json`);

    const paymentProvidersAccountsDb = await db.select("id").from("payment_provider_accounts");

    if (pixKeys.length > paymentProvidersAccountsDb.length)
        throw new Error("Key array size mismatch: Verify the number of accounts.");

    const keys = pixKeys.map((key, index) => ({
        ...key,
        payment_provider_account_id: paymentProvidersAccountsDb[index].id,
    }));

    await seedDb(tableName, keys);
}

async function seedPayments() {
    const tableName = "payments";

    const payments = await readDataFromJson(`${SEED_FILE_PATH}/payments.json`);

    const paymentProvidersAccountsDb = await db.select("id").from("payment_provider_accounts");

    const pixKeysDb = await db.select("id").from("pix_keys");

    if (payments.length > pixKeysDb.length || payments.length > paymentProvidersAccountsDb.length)
        throw new Error("Key array size mismatch: Verify the number of accounts.");

    const paymentsToDb = payments.map((payment, index) => ({
        ...payment,
        account_origin_id: paymentProvidersAccountsDb[index].id,
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

async function clearDb() {
    console.log("\n*****************************************");
    console.log("Deleting database tables...");
    await db("payments").del();
    await db("pix_keys").del();
    await db("payment_provider_accounts").del();
    await db("payment_provider_tokens").del();
    await db("payment_providers").del();
    await db("users").del();
    console.log("Database cleared successfully.");
    console.log("*****************************************\n");
}

await init();
