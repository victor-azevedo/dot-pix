import {faker} from "@faker-js/faker";
import {saveDataToJson} from "./utils.js";
import {UniqueEnforcer} from "enforce-unique";

const KEYS_LENGTH = 10_000;
const KEYS_FILE_PATH = "../Mocks/keys.json";

function createRandomKeys() {
    const uniqueEnforcerUudi = new UniqueEnforcer();

    function createKey() {
        const value = uniqueEnforcerUudi.enforce(() => {
            return faker.string.uuid();
        });
        const type = "Random";
        return {value, type};
    }

    const keys = []
    for (let i = 0; i < KEYS_LENGTH; i++) {
        keys.push(createKey());
    }
    return keys;
}

function init() {
    try {
        console.log("Creating accounts...");

        saveDataToJson(createRandomKeys(), KEYS_FILE_PATH);
    } catch
        (error) {
        console.error(`Error creating accounts: ${error}`);
    }
}

init()

