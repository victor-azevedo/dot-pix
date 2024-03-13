import http from "k6/http";
import {API_URL, defaultOptions, getSomeToken,} from "./helpers.js";

export const options = defaultOptions;

export default function () {
    const url = `${API_URL}/health`;
    const params = {
        headers: {
            Authorization: `Bearer ${getSomeToken()}`,
        },
    };

    http.get(url, params);
}