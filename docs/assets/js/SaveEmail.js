class SaveEmail {
    save(endpoint, email, onSucces, onError) {
        this.email = email;
        fetch(endpoint+'/api/email/save', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email: this.email }),
        })
            .then(async (response) => {
                if (!response.ok) {
                    const err = await response.json();
                    throw new Error(`${err.message}`);
                }
                return response.json();
            })
            .then((data) => {
                onSucces(data);
            })
            .catch((error) => {
                onError(error);
            });
    }
}