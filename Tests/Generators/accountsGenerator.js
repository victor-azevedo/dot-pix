import {faker} from "@faker-js/faker";
import {saveDataToJson, SEED_LENGTH} from "../utils.js";

const accountsLength = SEED_LENGTH;
const ACCOUNTS_FILE_PATH = "./Mocks/accounts.json";

function createRandomAccounts() {
    function createAccount() {
        const number = faker.string.numeric(5);
        const agency = faker.string.numeric(5);
        return {number, agency};
    }

    const accounts = []
    for (let i = 0; i < accountsLength; i++) {
        accounts.push(createAccount());
    }
    return accounts;
}

function init() {
    try {
        console.log("Creating accounts...");

        saveDataToJson(createRandomAccounts(), ACCOUNTS_FILE_PATH);
    } catch
        (error) {
        console.error(`Error creating accounts: ${error}`);
    }
}

init()

