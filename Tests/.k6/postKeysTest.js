import http from "k6/http";
import {
    API_URL,
    PAYLOAD_LENGTH,
    defaultOptions,
    usersSeed,
    getRandomToken,
    getRandomElement,
    parseJsonToArray
} from "./helpers.js";

const ACCOUNTS_FILE_PATH = "../Mocks/accounts.json";
const KEYS_FILE_PATH = "../Mocks/keys.json";

const payloads = generatePayloads();
export const options = defaultOptions;
export default function () {
    const url = `${API_URL}/keys`;
    const params = {
        headers: {
            'Authorization': `Bearer ${getRandomToken()}`,
            'Content-Type': 'application/json',
        },
    };

    const payload = JSON.stringify(getRandomElement(payloads));

    http.post(url, payload, params);
}


function generatePayloads() {
    console.log("Creating payloads...")
    const accounts = parseJsonToArray("accounts", ACCOUNTS_FILE_PATH);
    const keys = parseJsonToArray("keys", KEYS_FILE_PATH);

    if (accounts.length < PAYLOAD_LENGTH || keys.length < PAYLOAD_LENGTH || usersSeed.length < PAYLOAD_LENGTH)
        throw new Error('Small mock`s files');

    const payloads = []
    for (let i = 0; i < PAYLOAD_LENGTH; i++) {
        const key = getRandomElement(keys);
        const account = getRandomElement(accounts);
        const user = getRandomElement(usersSeed);
        payloads.push({
                "key":
                    {
                        "value":
                            key["value"],
                        "type":
                            key["type"]
                    }
                ,
                "user":
                    {
                        "cpf":
                            user["Cpf"]
                    }
                ,
                "account":
                    {
                        "number":
                            account["number"],
                        "agency":
                            account["agency"]
                    }
            }
        )
    }

    console.log("Payloads created successfully!")
    return payloads;
}