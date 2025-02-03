window.signInWithGoogle = async function () {
    const provider = new firebase.auth.GoogleAuthProvider();
    const result = await firebase.auth().signInWithPopup(provider);
    return result.user.uid;
}

window.signOut = async function () {
    await firebase.auth().signOut();
}

window.isUserAuthenticated = async function () {
    return new Promise((resolve) => {
        firebase.auth().onAuthStateChanged(user => {
            resolve(!!user);
        });
    });
}

window.getUserToken = async function () {
    const user = firebase.auth().currentUser;
    if (user) {
        return await user.getIdToken();
    }
    return null;
} 