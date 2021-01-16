import { Component, Mixins, Prop } from 'vue-property-decorator';
import { CardRank } from '@/models/custom';
import { BaseMixin } from '@/mixins/base-mixin';

@Component
export default class PlayingCard extends Mixins(BaseMixin)
{
    @Prop()
    customClasses?: Record<string, boolean>

    @Prop()
    rank?: number;

    @Prop()
    suit?: number;

    @Prop()
    selectable?: boolean;

    get classes(): Record<string, boolean>
    {
        return {
            'playing-card': true,
            'playing-card--selectable': !!this.selectable,
            [`playing-card--${this.suit}_${this.rank}`]: !!this.rank && !!this.suit,
            'playing-card--joker': this.rank === CardRank.Joker,
            ...this.customClasses
        };
    }

    formatLabel(): string
    {
        if (!this.suit && !this.rank)
        {
            return '';
        }

        return this.$translateAndCapitalize('labels.cardFormat',
            [
                this.$translate(`labels.cardRank${this.rank}`),
                this.$translate(`labels.cardSuit${this.suit}`)
            ]);
    }

    selectCard(): void
    {
        console.log('Card selected, selectable:', this.selectable);
        if (!this.selectable)
        {
            return;
        }

        this.$emit('select', this.suit, this.rank);
    }
}
