import { RequestModel } from "./request.model";

export class PortfolioModel {
    portfolioId: number;
    address: string;
    createDate: Date;

    requests: RequestModel[];
}