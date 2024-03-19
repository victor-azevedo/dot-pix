import { SharedArray } from "k6/data";
import http from "k6/http";
import { API_URL, defaultOptions, formatDate, isReportSummary } from "./helpers.js";

const routFileName = "getHealth";

const payloadFilePath = `../RequestsData/getKeys.json`;

const reportPath = `./.k6/reports/${formatDate()}-${routFileName}TestSummary.json`;

const token = new SharedArray("_", function () {
    return JSON.parse(open(payloadFilePath));
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

export function handleSummary(data) {
    if (isReportSummary) {
        return {
            [reportPath]: JSON.stringify(data),
        };
    }
}
