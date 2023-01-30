const apiUrl = '/api/bike';

export const getBikes = () => {
    return fetch(apiUrl)
        .then(resp => resp.json());
};

export const getBikeById = (id) => {
    return fetch(`${apiUrl}/${id}`)
        .then(resp => resp.json());
};

export const getBikesInShopCount = () => {
    return fetch(`${apiUrl}/count`)
        .then(resp => resp.json());
}