#include "embedded_resources.h"
#include <iostream>

int main() {
  const Embedded::Resource& icon = Embedded::get("test.jpg");

  std::cout << icon.size << std::endl;

  return 0; 
}
