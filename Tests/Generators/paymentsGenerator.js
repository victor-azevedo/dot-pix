import {faker} from "@faker-js/faker";
import {saveDataToJson, SEED_LENGTH, SEED_FILE_PATH} from "../utils.js"

const dataLength = SEED_LENGTH;
const filepath = `${SEED_FILE_PATH}/payments.json`

function createRandomPayments() {
    function createPayment() {
        const status = "SUCCESS"
        const amount = faker.number.int({min: 1, max: 9000000});
        const description = faker.word.adjective();
        return {
            "Status": status,
            "Amount": amount,
            "Description": description,
        };
    }

    const payments = []
    for (let i = 0; i < dataLength; i++) {
        payments.push(createPayment());
    }
    return payments;
}

export default async function createPaymentsJson() {
    try {
        const data = createRandomPayments();

        await saveDataToJson(data, filepath);
    } catch
        (error) {
        console.error(`Error creating data: ${error}`);
    }
}