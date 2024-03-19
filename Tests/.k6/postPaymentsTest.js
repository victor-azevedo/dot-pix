import { randomIntBetween, randomItem } from "https://jslib.k6.io/k6-utils/1.4.0/index.js";
import { check } from "k6";
import { SharedArray } from "k6/data";
import http from "k6/http";
import { API_URL, defaultOptions, formatDate, isReportSummary } from "./helpers.js";

const routFileName = "postPayments";

const payloadFilePath = `../RequestsData/${routFileName}.json`;

const reportPath = `./.k6/reports/${formatDate()}-${routFileName}TestSummary.json`;

const requests = new SharedArray("_", function () {
    return JSON.parse(open(payloadFilePath));
});

export const options = defaultOptions;

export default function () {
    const { token, payload } = randomItem(requests);

    const copiedPayload = JSON.parse(JSON.stringify(payload));

    copiedPayload.amount = randomIntBetween(1, 1000000);

    const url = `${API_URL}/payments`;
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
    });
}

export function handleSummary(data) {
    if (isReportSummary) {
        return {
            [reportPath]: JSON.stringify(data),
        };
    }
}
