export const API_URL = "http://localhost:5200";

export const defaultOptions = {
    vus: 10,
    duration: "30s",
};

export function getRandomElement(array) {
    return array[Math.floor(Math.random() * array.length)];
}
