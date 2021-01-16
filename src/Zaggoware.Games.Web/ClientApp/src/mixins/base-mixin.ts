import { Component, Vue } from 'vue-property-decorator';

@Component
export class BaseMixin extends Vue
{
    $translate(text: string, variables?: string[]): string
    {
        return this.$t(text, variables) as string;
    }

    $translateAndCapitalize(text: string, variables?: string[]): string
    {
        return this.$options.filters.capitalize(this.$translate(text, variables));
    }
}
