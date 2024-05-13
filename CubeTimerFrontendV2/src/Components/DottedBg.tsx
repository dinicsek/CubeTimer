function DottedBg(props: { density: string; sizeX: string; sizeY: string; opacity: string }) {
    return (
        <div
            style={{
                position: "absolute",
                width: props.sizeX,
                height: props.sizeY,
                backgroundImage: "radial-gradient(circle, #ffffff 3px, transparent 0)",
                backgroundSize: `${props.density}px ${props.density}px`,
                opacity: props.opacity,
                zIndex: 10,
            }}
        />
    );
}

export default DottedBg;
