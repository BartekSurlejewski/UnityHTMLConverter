async function init() {
    const data = SCENE_DATA;
    const scene = new THREE.Scene();

    const camera = new THREE.PerspectiveCamera(
        60,
        window.innerWidth / window.innerHeight,
        0.1,
        1000
    );

    camera.position.set(
        data.Camera.Position.x,
        data.Camera.Position.y,
        -data.Camera.Position.z
    );
    camera.quaternion.set(
        data.Camera.Rotation.x,
        data.Camera.Rotation.y,
        -data.Camera.Rotation.z,
        -data.Camera.Rotation.w
    );

    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth, window.innerHeight);

    document.body.appendChild(renderer.domElement);

    const cubes = [];

    for (let c of data.Cubes) {

        const geo = new THREE.BoxGeometry(1, 1, 1);
        const mat = new THREE.MeshNormalMaterial();
        const cube = new THREE.Mesh(geo, mat);

        cube.position.set(c.Position.x, c.Position.y, -c.Position.z);

        cube.rotation.order = "YXZ";
        cube.rotation.set(
            THREE.MathUtils.degToRad(-c.Rotation.x),
            THREE.MathUtils.degToRad(-c.Rotation.y),
            THREE.MathUtils.degToRad(c.Rotation.z)
        );

        cube.scale.order = "XYZ";
        cube.scale.set(c.Scale.x, c.Scale.y, c.Scale.z);

        cubes.push(cube);
    }

    for (let i = 0; i < cubes.length; i++) {
        const parentIndex = data.Cubes[i].ParentIndex;

        if (parentIndex !== -1) {
            cubes[parentIndex].add(cubes[i]); // add to parent cube
        } else {
            scene.add(cubes[i]);              // add to scene root
        }
    }
    
    renderer.render(scene, camera);
}

init();
