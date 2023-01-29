export interface RecipeGetResponseData {
    id: string;
    title: string;
    timestamp: Date;
    ingredients: string[];
    steps: string[];
}

export interface RecipeGetResponse {
    success: boolean;
    message: string;
    recipes?: RecipeGetResponseData[];
}
