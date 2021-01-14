export default function capitalize(value: string): string
{
    if (value.length <= 1)
    {
        return value.toUpperCase();
    }

    return value[0].toUpperCase() + value.substr(1);
}
