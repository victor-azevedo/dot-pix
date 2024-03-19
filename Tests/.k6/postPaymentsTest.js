import { check } from "k6";
import { SharedArray } from "k6/data";
import http from "k6/http";
import { API_URL, defaultOptions, getRandomElement } from "./helpers.js";

const PAYLOAD_FILE_PATH = "../RequestsData/postPayments.json";

const requests = new SharedArray("_", function () {
    return JSON.parse(open(PAYLOAD_FILE_PATH));
});

export const options = defaultOptions;

export default function () {
    const { token, payload } = getRandomElement(requests);

    const url = `${API_URL}/payments`;
    const params = {
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        },
    };

    const payloadStr = JSON.stringify(payload);

    const res = http.post(url, payloadStr, params);

    check(res, {
        "is not conflict": (r) => r.status != 409,
    });
}
