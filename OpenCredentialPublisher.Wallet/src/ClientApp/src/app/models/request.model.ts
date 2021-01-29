import { TestModel } from "./test.model";

export class RequestModel {
    requestId: number;
    requestDate: Date;
    transactionId: string;
    ipfsHash: string;

    tests: TestModel[];
}