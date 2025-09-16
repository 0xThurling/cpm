#ifndef EMBEDDED_RESOURCES_H
#define EMBEDDED_RESOURCES_H

#include <string>
#include <cstddef>

namespace Embedded {
    struct Resource {
        const unsigned char* data;
        size_t size;
    };

    const Resource& get(const std::string& name);
}

#endif // EMBEDDED_RESOURCES_H
