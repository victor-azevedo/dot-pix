import { randomItem } from "https://jslib.k6.io/k6-utils/1.4.0/index.js";
import { SharedArray } from "k6/data";
import http from "k6/http";
import { API_URL, defaultOptions, formatDate, isReportSummary } from "./helpers.js";

const routFileName = "getKeys";

const payloadFilePath = `../RequestsData/${routFileName}.json`;

const reportPath = `./.k6/reports/${formatDate()}-${routFileName}TestSummary.json`;

const requests = new SharedArray("_", function () {
    return JSON.parse(open(payloadFilePath));
});

export const options = defaultOptions;

export default function () {
    const { token, payload } = randomItem(requests);

    const { type, value } = payload;

    const url = `${API_URL}/keys/${type}/${value}`;
    const params = {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    };

    http.get(url, params);
}

export function handleSummary(data) {
    if (isReportSummary) {
        return {
            [reportPath]: JSON.stringify(data),
        };
    }
}
