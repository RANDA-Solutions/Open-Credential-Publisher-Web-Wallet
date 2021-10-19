declare module server {
	interface logModel {
		id: number;
		message: string;
		messageTemplate: string;
		level: string;
		timestamp: Date;
		exception: string;
		properties: string;
		logEvent: string;
	}
}
