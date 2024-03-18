import {faker} from "@faker-js/faker";
import {saveDataToJson, SEED_LENGTH, SEED_FILE_PATH} from "../utils.js"

const dataLength = SEED_LENGTH;
const filepath = `${SEED_FILE_PATH}/paymentProviders.json`

function createRandomPaymentProviders() {
    function createPaymentProvider() {
        const name = faker.company.name();
        const apiUrl = "http://pspapi:8081";

        return {
            "Name": name,
            "ApiUrl": apiUrl,
        };
    }

    const paymentProviders = []
    for (let i = 0; i < dataLength; i++) {
        paymentProviders.push(createPaymentProvider());
    }

    return paymentProviders;
}

export default async function createPaymentProvidersJson() {
    try {
        const data = createRandomPaymentProviders();
        await saveDataToJson(data, filepath);
    } catch
        (error) {
        console.error(`Error creating data: ${error}`);
    }
}