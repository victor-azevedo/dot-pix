import { SharedArray } from "k6/data";
import http from "k6/http";
import { API_URL, defaultOptions } from "./helpers.js";

const PAYLOAD_FILE_PATH = "../RequestsData/getKeys.json";

const token = new SharedArray("_", function () {
    return JSON.parse(open(PAYLOAD_FILE_PATH));
})[0].token;

export const options = defaultOptions;

export default function () {
    const url = `${API_URL}/health`;
    const params = {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    };

    http.get(url, params);
}
