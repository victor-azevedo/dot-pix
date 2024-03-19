import { faker } from "@faker-js/faker";
import { UniqueEnforcer } from "enforce-unique";
import { SEED_FILE_PATH, SEED_LENGTH, saveDataToJson } from "../utils.js";

const dataLength = SEED_LENGTH;
const filepath = `${SEED_FILE_PATH}/payments.json`;

function createRandomPayments() {
    const uniqueEnforcerUudi = new UniqueEnforcer();

    function createPayment() {
        const id = uniqueEnforcerUudi.enforce(() => {
            return faker.string.uuid();
        });
        const status = "SUCCESS";
        const amount = faker.number.int({ min: 1, max: 9000000 });
        const description = faker.word.adjective();
        return {
            id,
            status,
            amount,
            description,
        };
    }

    const payments = [];
    for (let i = 0; i < dataLength; i++) {
        payments.push(createPayment());
    }
    return payments;
}

export default async function createPaymentsJson() {
    try {
        const data = createRandomPayments();

        await saveDataToJson(data, filepath);
    } catch (error) {
        console.error(`Error creating data: ${error}`);
    }
}
