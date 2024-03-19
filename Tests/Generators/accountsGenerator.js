import { faker } from "@faker-js/faker";
import { SEED_FILE_PATH, SEED_LENGTH, saveDataToJson } from "../utils.js";

const dataLength = SEED_LENGTH;
const filepath = `${SEED_FILE_PATH}/accounts.json`;

function createRandomAccounts() {
    function createAccount() {
        const account = faker.string.numeric(5);
        const agency = faker.string.numeric(5);
        return { account, agency };
    }

    const accounts = [];
    for (let i = 0; i < dataLength; i++) {
        accounts.push(createAccount());
    }
    return accounts;
}

export default async function createAccountsJson() {
    try {
        const data = createRandomAccounts();

        await saveDataToJson(data, filepath);
    } catch (error) {
        console.error(`Error creating data: ${error}`);
    }
}
