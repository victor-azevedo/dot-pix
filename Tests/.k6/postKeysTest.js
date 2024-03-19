import { uuidv4 } from "https://jslib.k6.io/k6-utils/1.4.0/index.js";
import { check } from "k6";
import { SharedArray } from "k6/data";
import http from "k6/http";
import { API_URL, defaultOptions, getRandomElement } from "./helpers.js";

const PAYLOAD_FILE_PATH = "../RequestsData/postKeys.json";

const requests = new SharedArray("_", function () {
    return JSON.parse(open(PAYLOAD_FILE_PATH));
});

export const options = defaultOptions;

export default function () {
    const { token, payload } = getRandomElement(requests);

    const copiedPayload = JSON.parse(JSON.stringify(payload));

    const randomUUID = uuidv4();
    copiedPayload.key.value = randomUUID;

    const url = `${API_URL}/keys`;
    const params = {
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        },
    };

    const payloadStr = JSON.stringify(copiedPayload);

    const res = http.post(url, payloadStr, params);

    check(res, {
        "is not conflict": (r) => r.status != 409,
        "is not repeated key": (r) => !r.body.includes("The key value is already in use"),
    });
}
