import {faker} from "@faker-js/faker";
import {UniqueEnforcer} from "enforce-unique";
import {saveDataToJson} from "./utils.js"

const USERS_LENGTH = 10_000;
const PAYMENT_PROVIDES_LENGTH = 10_000;

export const SEED_TEST_FILE_PATH = "../../Database/seedTest.json"

function createRandomUsers() {
    const uniqueEnforcerCpf = new UniqueEnforcer();

    function createUser() {
        const cpf = uniqueEnforcerCpf.enforce(() => {
            return faker.string.numeric(11);
        });
        const name = faker.person.fullName();
        return {
            "Name": name,
            "Cpf": cpf,
        };
    }

    const users = []
    for (let i = 0; i < USERS_LENGTH; i++) {
        users.push(createUser());
    }
    return users;
}

function createRandomPaymentProviders() {
    const uniqueEnforcerUudi = new UniqueEnforcer();

    function createPaymentProvider() {
        const name = faker.company.name();
        const token = uniqueEnforcerUudi.enforce(() => {
            return faker.string.uuid();
        });
        return {
            "Name": name,
            "Token": token,
        };
    }

    const paymentProviders = []
    for (let i = 0; i < PAYMENT_PROVIDES_LENGTH; i++) {
        paymentProviders.push(createPaymentProvider());
    }

    return paymentProviders;
}

function init() {
    try {
        console.log("Creating seed...");

        const seed = {
            "Users": createRandomUsers(),
            "PaymentProviders": createRandomPaymentProviders()
        }

        saveDataToJson(seed, SEED_TEST_FILE_PATH);
    } catch
        (error) {
        console.error(`Error creating seed: ${error}`);
    }
}

init()

