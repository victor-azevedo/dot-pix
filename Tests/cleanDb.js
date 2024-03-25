import * as readline from 'readline';
import * as dotenv from "dotenv";
import knex from "knex";

dotenv.config();

const db = knex({
    client: "pg",
    connection: process.env.DATABASE_URL,
});

async function init() {
    try {
        const confirm = await confirmClearDatabase();
        if (confirm) {
            await clearDb();
        } else {
            console.log("Database cleaning cancelled.");
        }
    } catch (error) {
        console.error("An error occurred during database cleaning:", error);
    } finally {
        await db.destroy();
    }
}

async function confirmClearDatabase() {
    return new Promise((resolve, reject) => {
        const rl = readline.createInterface({
            input: process.stdin,
            output: process.stdout
        });
        rl.question(`Are you sure you want to clear the database? This action cannot be undone. (y/N): `, (answer) => {
            rl.close();
            if (answer.toLowerCase() === 'y') {
                resolve(true);
            } else {
                resolve(false);
            }
        });
    });
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

async function getDatabaseNameFromUrl() {
    const matches = process.env.DATABASE_URL.match(/\/([a-zA-Z0-9_-]+)$/);
    if (matches && matches.length > 1) {
        return matches[1];
    } else {
        throw new Error("Unable to parse database name from connection string.");
    }
}

async function showDatabaseName() {
    try {
        const dbName = await getDatabaseNameFromUrl();
        console.log(`Using database: ${dbName}`);
    } catch (error) {
        console.error("An error occurred while fetching the database name:", error);
    }
}

await showDatabaseName();
await init();
