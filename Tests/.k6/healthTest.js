import http from "k6/http";
import {API_URL, getRandomToken, defaultOptions,} from "./helpers.js";

export const options = defaultOptions;

export default function () {
    const url = `${API_URL}/health`;
    const params = {
        headers: {
            Authorization: `Bearer ${getRandomToken()}`,
        },
    };

    http.get(url, params);
}