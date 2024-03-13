import http from "k6/http";
import {
    API_URL,
    PAYLOAD_LENGTH,
    defaultOptions,
    getRandomToken,
    getRandomElement,
    parseJsonToArray,
} from "./helpers.js";

const KEYS_FILE_PATH = "../Mocks/keys.json";

export const options = defaultOptions;

const payloads = getKeyPayloadsGenerator(PAYLOAD_LENGTH);

const token = getRandomToken();

export default function () {
    const {type, value} = getRandomElement(payloads);

    const url = `${API_URL}/keys/${type}/${value}`;
    const params = {
        headers: {
            'Authorization': `Bearer ${token}`
        },
    };

    http.get(url, params);
}

function getKeyPayloadsGenerator(length) {
    console.log("Creating payload...");
    const keys = parseJsonToArray("keys", KEYS_FILE_PATH);

    if (keys.length < length)
        throw new Error('Small mock`s files');

    const payloads = []
    for (let i = 0; i < length; i++) {
        const {type, value} = getRandomElement(keys);
        payloads.push({type, value});
    }

    console.log("Payloads created successfully!")
    return payloads;
}
