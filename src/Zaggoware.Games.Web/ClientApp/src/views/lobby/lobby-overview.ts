import { Component, Mixins } from 'vue-property-decorator';
import { BaseMixin } from '@/mixins/base-mixin';
import { GameLobby } from '@/models/custom';
import { GameLobbyHub } from '@/hubs/game-lobby-hub';

@Component
export default class LobbyOverview extends Mixins(BaseMixin)
{
    hub: GameLobbyHub;
    lobbies: GameLobby[] = [];

    async created(): Promise<void>
    {
        this.hub = new GameLobbyHub(this.$route.query.id as string, this.$route.query.password as string);
        await this.hub.start();
        await this.fetchLobbies();
    }

    async fetchLobbies(): Promise<void>
    {
        this.lobbies = await this.hub.fetchLobbies();
    }
}
