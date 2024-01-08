/** @type {import('tailwindcss').Config} */
export default {
    content: [
        './src/**/index.html',
        './src/**/*.tsx'
    ],
    theme: {
        extend: {
            keyframes: {
                pan: {

                }
            }
        },
    },
    plugins: [
        'tailwindcss-hero-patterns',
    ],
}

