export const API_URL = "http://localhost:8080";

export const isReportSummary = false;

export const defaultOptions = {
    vus: 20,
    duration: "30s",
};

export function getRandomElement(array) {
    return array[Math.floor(Math.random() * array.length)];
}

export function formatDate(date = new Date()) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hour = String(date.getHours()).padStart(2, "0");
    const minute = String(date.getMinutes()).padStart(2, "0");
    return `${year}-${month}-${day} ${hour}:${minute}`;
}
