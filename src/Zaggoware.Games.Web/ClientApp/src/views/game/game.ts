import { Component, Mixins } from 'vue-property-decorator';
import { BaseMixin } from '@/mixins/base-mixin';
import { GameMixin } from '@/mixins/game-mixin';

@Component
export default class GameView extends Mixins(BaseMixin, GameMixin)
{
    async quitGame(): Promise<void>
    {
        await this.$router.replace('/');
    }
}
