export interface Response<TResponse>{
    data: TResponse;
    errors?: ResponseError[];
}

interface ResponseError{
    code: string,
    description?: string,
    meta?: { field?: string };
}