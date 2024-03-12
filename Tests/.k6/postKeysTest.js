import http from "k6/http";
import {API_URL, defaultOptions, usersSeed, getRandomToken, parseJsonToArray} from "./helpers.js";

const ACCOUNTS_FILE_PATH = "../Mocks/accounts.json";
const KEYS_FILE_PATH = "../Mocks/keys.json";
const PAYLOAD_LENGTH = 10000;

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

    const payload = JSON.stringify(getPayload());

    http.post(url, payload, params);
}


function getPayload() {
    return payloads.pop();
}

function generatePayloads() {
    console.log("Creating payloads...")
    const accounts = [...parseJsonToArray("accounts", ACCOUNTS_FILE_PATH)];
    const keys = [...parseJsonToArray("keys", KEYS_FILE_PATH)];
    const users = [...usersSeed];

    if (accounts.length < PAYLOAD_LENGTH || keys.length < PAYLOAD_LENGTH || users.length < PAYLOAD_LENGTH)
        throw new Error('Small mock`s files');

    const payloads = []
    for (let i = 0; i < PAYLOAD_LENGTH; i++) {
        const key = keys.pop();
        const account = accounts.pop();
        const user = users.pop();
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