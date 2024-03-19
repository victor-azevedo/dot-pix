import { SharedArray } from "k6/data";
import http from "k6/http";
import { API_URL, defaultOptions, getRandomElement } from "./helpers.js";

const PAYLOAD_FILE_PATH = "../RequestsData/getKeys.json";

const requests = new SharedArray("_", function () {
    return JSON.parse(open(PAYLOAD_FILE_PATH));
});

export const options = defaultOptions;

export default function () {
    const { token, payload } = getRandomElement(requests);

    const { type, value } = payload;

    const url = `${API_URL}/keys/${type}/${value}`;
    const params = {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    };

    http.get(url, params);
}
