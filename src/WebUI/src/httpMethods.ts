/**
 * Status and response body from request
 */
export type FetchResponse<T> = {
    body: T;
    status: number;
}


/**
 * Used as body field in the FetchResponse
 */
export type DataArrayResponse<T> = {
    data: T[];
    errors: string[];
}



export async function httpGet(route: string): Promise<FetchResponse<null>> {
    let response = await fetch(route, {
        method: "GET",
        headers: {
            'Content-Type': 'application/json'
        },
    });
    return {body: null, status: response.status};
}

export async function httpGetWithBody<T>(route: string): Promise<FetchResponse<T>> {
    let response = await fetch(route ,{
        method: "GET",
        headers: {
            'Content-Type': 'application/json'
        },
    });
    let body = await response.json();
    return {body: body, status: response.status};
}

export async function httpPost<T>(route: string, data: T): Promise<FetchResponse<null>> {
    let response = await fetch(route, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
    return {body: null, status: response.status};
}

export async function httpPostWithBody<TIn, TOut>(route: string, data: TIn): Promise<FetchResponse<TOut>> {
    let response = await fetch(route, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
    let body = await response.json();
    return {body: body, status: response.status};
}
