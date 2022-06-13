export class Favorite {
    id: string;
    userId: string;
    lotId: number;

    constructor(id: string, userId: string, lotId: number) {
        this.id = id;
        this.userId = userId;
        this.lotId = lotId;
    }
}