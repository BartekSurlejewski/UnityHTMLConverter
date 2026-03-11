function init() {
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

    const sceneObjects = [];

    for (let c of data.SceneObjects) {

        let sceneObject;

        if ((c.PrimitiveType == 0)) {   // Empty object
            sceneObject = new THREE.Object3D();
        }
        if (c.PrimitiveType == 1) {   // A cube
            const geo = new THREE.BoxGeometry(1, 1, 1);
            const mat = new THREE.MeshNormalMaterial();
            sceneObject = new THREE.Mesh(geo, mat);
        }

        sceneObject.name = c.Name;
        sceneObject.position.set(c.Position.x, c.Position.y, -c.Position.z);
        sceneObject.rotation.order = "YXZ";
        sceneObject.rotation.set(
            THREE.MathUtils.degToRad(-c.Rotation.x),
            THREE.MathUtils.degToRad(-c.Rotation.y),
            THREE.MathUtils.degToRad(c.Rotation.z)
        );

        sceneObject.scale.order = "XYZ";
        sceneObject.scale.set(c.Scale.x, c.Scale.y, c.Scale.z);

        sceneObjects.push(sceneObject);
    }

    for (let i = 0; i < sceneObjects.length; i++) {
        const parentIndex = data.SceneObjects[i].ParentIndex;

        if (parentIndex !== -1) {
            sceneObjects[parentIndex].add(sceneObjects[i]); // add to parent object
        } else {
            scene.add(sceneObjects[i]);              // add to scene root
        }
    }

    renderer.render(scene, camera);
}

init();
