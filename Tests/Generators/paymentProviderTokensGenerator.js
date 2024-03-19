import { faker } from "@faker-js/faker";
import { UniqueEnforcer } from "enforce-unique";
import { saveDataToJson, SEED_FILE_PATH, SEED_LENGTH } from "../utils.js";

const dataLength = SEED_LENGTH;
const filepath = `${SEED_FILE_PATH}/paymentProviderTokens.json`;

function createRandomPaymentProviderToken() {
    const uniqueEnforcerUudi = new UniqueEnforcer();

    function createPaymentProviderToken() {
        return uniqueEnforcerUudi.enforce(() => {
            return faker.string.uuid();
        });
    }

    const paymentProviderTokens = [];
    for (let i = 0; i < dataLength; i++) {
        paymentProviderTokens.push(createPaymentProviderToken());
    }

    return paymentProviderTokens;
}

export default async function createPaymentProviderTokensJson() {
    try {
        const data = createRandomPaymentProviderToken();
        await saveDataToJson(data, filepath);
    } catch (error) {
        console.error(`Error creating data: ${error}`);
    }
}
