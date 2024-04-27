import { setupSdk } from "dissonity";

window.addEventListener("DOMContentLoaded", () => {

  setupSdk({
    clientId: 1232014118995628122,
    scope: ["rpc.voice.read", "guilds.members.read"],
    tokenRoute: "/api/token"
  });
});