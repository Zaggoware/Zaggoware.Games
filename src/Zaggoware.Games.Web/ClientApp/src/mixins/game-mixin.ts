import { Component, Vue } from 'vue-property-decorator';
import { Keys } from '@/utils/enums/keys';

@Component
export class GameMixin extends Vue
{
    showMenuModal = false;
    menuModalActivationKeys = [Keys.Escape, Keys.Pause];
    keysDown: Keys[] = [];

    created(): void
    {
        document.addEventListener('keydown', this.onDocumentKeyDown);
        document.addEventListener('keyup', this.onDocumentKeyUp);
    }

    isKeyDown(key: Keys): boolean
    {
        return this.keysDown.indexOf(key) >= 0;
    }

    isKeyUp(key: Keys): boolean
    {
        return this.keysDown.indexOf(key) < 0;
    }

    onDocumentKeyDown($evt: KeyboardEvent): boolean
    {
        if (this.isKeyUp($evt.keyCode))
        {
            this.keysDown.push($evt.keyCode);

            if (this.menuModalActivationKeys.indexOf($evt.keyCode) >= 0)
            {
                this.showMenuModal = !this.showMenuModal;
            }
        }

        return true;
    }

    onDocumentKeyUp($evt: KeyboardEvent): boolean
    {
        const index = this.keysDown.indexOf($evt.keyCode);
        if (index >= 0)
        {
            this.keysDown.splice(index, 1);
        }

        return true;
    }
}
