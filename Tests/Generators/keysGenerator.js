import { faker } from "@faker-js/faker";
import { UniqueEnforcer } from "enforce-unique";
import { SEED_FILE_PATH, SEED_LENGTH, saveDataToJson } from "../utils.js";

const dataLength = SEED_LENGTH;
const filepath = `${SEED_FILE_PATH}/keys.json`;

function createRandomKeys() {
    const uniqueEnforcerUudi = new UniqueEnforcer();

    function createKey() {
        const value = uniqueEnforcerUudi.enforce(() => {
            return faker.string.uuid();
        });
        const type = "Random";
        return { value, type };
    }

    const keys = [];
    for (let i = 0; i < dataLength; i++) {
        keys.push(createKey());
    }
    return keys;
}

export default async function createKeysJson() {
    try {
        const data = createRandomKeys();

        await saveDataToJson(data, filepath);
    } catch (error) {
        console.error(`Error creating data: ${error}`);
    }
}
