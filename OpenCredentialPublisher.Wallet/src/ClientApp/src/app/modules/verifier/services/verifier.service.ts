export class VerifierService {

	setCreator(id: string) {
		try {
		localStorage.setItem(`proof${id}`, true.toString());
		}
		catch(err) {
			console.log(err);
		}
	}

	isCreator(id: string) {
		let value = localStorage.getItem(`proof${id}`);
		return value === "true";
	}

	setAddress(address: string) {
		try {
		localStorage.setItem(`verifier_address`, address);
		}
		catch(err) {
			console.log(err);
		}
	}

	getAddress() {
		let value = localStorage.getItem(`verifier_address`);
		return value;
	}
}