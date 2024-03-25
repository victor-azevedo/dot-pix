export const API_URL = "http://localhost:8080";

export const isReportSummary = false;

export const defaultOptions = {
        scenarios: {
            normal_usage: {
                executor: "shared-iterations",
                vus: 40,
                iterations: 5000,
                maxDuration: "60s",
            }
        },
        thresholds: {
            http_req_failed: ["rate<0.02"],
            http_req_duration:
                ["p(95)<500"],
        }
        ,
    }
;

export function formatDate(date = new Date()) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hour = String(date.getHours()).padStart(2, "0");
    const minute = String(date.getMinutes()).padStart(2, "0");
    return `${year}-${month}-${day} ${hour}:${minute}`;
}
